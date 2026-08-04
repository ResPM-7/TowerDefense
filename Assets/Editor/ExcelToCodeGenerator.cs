#if UNITY_EDITOR
using NPOI.HSSF.UserModel;
// ★ 유니티 엑셀 플러그인에 내장된 라이브러리를 활용합니다!
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions; // ★ C# 클래스 이름 변환용 정규식 라이브러리 추가
using UnityEditor;
using UnityEngine;

public class ExcelToCodeGenerator : EditorWindow
{
    // CSV(TextAsset)가 아닌, 엑셀 파일 원본(DefaultAsset)을 받습니다.
    private DefaultAsset excelFile;

    [MenuItem("Tools/엑셀 -> C# 완전 자동 생성기")]
    public static void ShowWindow()
    {
        GetWindow<ExcelToCodeGenerator>("엑셀 자동 툴");
    }

    private void OnGUI()
    {
        GUILayout.Label("엑셀 다중 시트 -> C# 클래스 변환기", EditorStyles.boldLabel);
        GUILayout.Space(10);

        // 엑셀 파일 등록 슬롯
        excelFile = (DefaultAsset)EditorGUILayout.ObjectField("엑셀 파일 (.xlsx)", excelFile, typeof(DefaultAsset), false);

        GUILayout.Space(20);

        if (GUILayout.Button("모든 시트 C# 스크립트로 자동 생성", GUILayout.Height(40)))
        {
            if (excelFile == null)
            {
                EditorUtility.DisplayDialog("경고", "엑셀 파일을 먼저 넣어주세요", "확인");
                return;
            }

            GenerateClassesFromExcel();
        }
    }

    private void GenerateClassesFromExcel()
    {
        // 유니티 프로젝트 내의 엑셀 파일 실제 경로를 가져옵니다.
        string assetPath = AssetDatabase.GetAssetPath(excelFile);

        IWorkbook workbook = null;

        try
        {
            // FileShare.ReadWrite로 열면 엑셀 파일이 켜져 있어도 읽을 수 있습니다.
            using (FileStream stream = new FileStream(assetPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                if (assetPath.EndsWith(".xlsx"))
                    workbook = new XSSFWorkbook(stream);
                else if (assetPath.EndsWith(".xls"))
                    workbook = new HSSFWorkbook(stream);
                else
                {
                    Debug.LogError("엑셀 파일(.xlsx, .xls)만 가능합니다");
                    return;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("엑셀 파일을 여는 중 오류 발생 (엑셀을 끄고 해보세요) : " + e.Message);
            return;
        }

        // 폴더가 없으면 생성 (Assets/Scripts/Data 폴더에 모아둡니다)
        string folderPath = Application.dataPath + "/Scripts/Data";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        int generatedCount = 0;

        // ★ 추가: 생성된 시트 이름들을 모아둘 리스트
        List<string> validSheetNames = new List<string>();

        // ★ 안전장치: 엑셀 파일 이름에서 C# 클래스로 쓸 수 있는 안전한 이름만 추출 (공백, 괄호 등 제거)
        // 예시: "TowerDefenseDB (1)" -> "TowerDefenseDB1" 로 자동 변환
        string safeDbClassName = Regex.Replace(excelFile.name, @"[^a-zA-Z0-9_]", "");

        // C# 클래스 이름은 숫자로 시작할 수 없으므로 방어 코드 추가
        if (!string.IsNullOrEmpty(safeDbClassName) && char.IsDigit(safeDbClassName[0]))
        {
            safeDbClassName = "_" + safeDbClassName;
        }

        // ★ 핵심: 엑셀 파일 안에 있는 '모든 시트'를 반복문으로 돕니다.
        for (int i = 0; i < workbook.NumberOfSheets; i++)
        {
            ISheet sheet = workbook.GetSheetAt(i);

            // 시트 이름 가져오기 (예: "Tower", "Enemy", "Mission")
            string sheetName = sheet.SheetName;

            // 시트 데이터가 비어있으면 건너뜀
            if (sheet.LastRowNum < 1) continue;

            // 시트 이름을 바탕으로 클래스 생성 함수 호출 (이름 뒤에 Entity를 자동으로 붙여줍니다)
            bool success = GenerateClassForSheet(sheet, folderPath, sheetName);
            if (success)
            {
                generatedCount++;
                validSheetNames.Add(sheetName); // ★ 성공적으로 만들어진 시트 이름을 명단에 추가
            }
        }

        // ★ 수정: 하드코딩된 이름 대신, 엑셀 파일명 기반으로 만든 안전한 클래스 이름을 넘겨줍니다!
        GenerateDBClass(validSheetNames, folderPath, safeDbClassName);

        // 생성 완료 후 유니티 에디터 새로고침
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("완료", $"총 {generatedCount}개의 시트가 스크립트로 자동 생성되었습니다!", "확인");
    }

    private bool GenerateClassForSheet(ISheet sheet, string folderPath, string sheetName)
    {
        // 1행(이름)과 2행(타입)을 가져옵니다. (인덱스는 0부터 시작)
        IRow nameRow = sheet.GetRow(0);
        IRow typeRow = sheet.GetRow(1);

        if (nameRow == null || typeRow == null) return false;

        // 시트명 + Entity 로 클래스 이름을 강제 지정합니다. (예: Tower -> TowerEntity)
        string className = sheetName + "Entity";

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("using System;");
        sb.AppendLine("using UnityEngine;");
        sb.AppendLine();
        sb.AppendLine("[Serializable]");
        sb.AppendLine($"public class {className}");
        sb.AppendLine("{");

        // 가로(열) 개수만큼 반복하며 변수를 생성합니다.
        for (int j = 0; j < nameRow.LastCellNum; j++)
        {
            ICell nameCell = nameRow.GetCell(j);
            ICell typeCell = typeRow.GetCell(j);

            if (nameCell == null || typeCell == null) continue;

            string vName = nameCell.ToString().Trim();
            string vType = typeCell.ToString().Trim();

            // 빈칸이거나 타입이 안 적혀 있으면 무시 (nextUpgradeDataNum 이후에 텅 빈 열들 방어)
            if (string.IsNullOrEmpty(vName) || string.IsNullOrEmpty(vType)) continue;

            sb.AppendLine($"    public {vType} {vName};");
        }

        sb.AppendLine("}");

        // C# 파일 생성 및 저장
        string savePath = folderPath + $"/{className}.cs";
        File.WriteAllText(savePath, sb.ToString(), Encoding.UTF8);

        Debug.Log($"<color=cyan>[자동 생성됨]</color> 시트: {sheetName} -> 스크립트: {className}.cs");
        return true;
    }

    // ★ 수정된 함수: 매개변수로 dbClassName을 받아 동적으로 생성합니다.
    private void GenerateDBClass(List<string> sheetNames, string folderPath, string dbClassName)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using UnityEngine;");
        sb.AppendLine();
        sb.AppendLine("[ExcelAsset]");

        // 동적으로 넘어온 파일 이름을 클래스 이름으로 사용합니다!
        sb.AppendLine($"public class {dbClassName} : ScriptableObject");
        sb.AppendLine("{");

        // 모아둔 시트 이름들을 바탕으로 List 변수를 쫙 생성합니다.
        foreach (string sheetName in sheetNames)
        {
            sb.AppendLine($"\tpublic List<{sheetName}Entity> {sheetName};");
        }

        sb.AppendLine("}");

        // 생성된 이름으로 .cs 파일 저장
        string savePath = folderPath + $"/{dbClassName}.cs";
        File.WriteAllText(savePath, sb.ToString(), Encoding.UTF8);

        Debug.Log($"<color=yellow>[자동 생성됨]</color> 데이터베이스 스크립트: {dbClassName}.cs");
    }
}
#endif
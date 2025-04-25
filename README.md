# Sheet2SO 📄 ➡️ 🎮

**Google Spreadsheet를 Unity의 ScriptableObject로 변환하여 프로젝트에서 구조화된 데이터로 활용할 수 있도록 돕는 유틸리티 패키지입니다.**

---

## ✨ 주요 기능

- Google Spreadsheet의 CSV Export → Unity ScriptableObject 자동 변환
- 구조체 기반 데이터 정의 및 자동 생성
- ScriptTemplate을 통한 커스텀 템플릿 확장 가능
- Git 기반 Unity Package로 손쉬운 프로젝트 통합

---

## **📦 설치 방법**  

### **manifest.json을 이용한 설치**  
Unity 프로젝트의 `Packages/manifest.json` 파일에 다음 내용을 추가하세요.  
```json
"dependencies": {
  "com.dopple.uiextendbutton": "https://github.com/doppleddiggong/Sheet2SO.git"
}
```

### **Git URL을 이용한 설치**  
- Click **Window** > **Package Manager** to **open Package Manager UI.**
- Click **+** > **Add package from git URL**... and input the repository URL : [](https://github.com/doppleddiggong/Sheet2SO.git)
    
    [https://github.com/doppleddiggong/Sheet2SO.git](https://github.com/doppleddiggong/Sheet2SO.git)

![image.png](https://github.com/user-attachments/assets/a19a7528-aa17-4964-a7bf-c8727faa1d08)

---

📁 샘플
/Example 폴더안에 Sample데이터 UnityPackage가 있습니다.
해당 패키지 를 통해 샘플을 확인할 수 있습니다

/Sample/TSV/ 폴더에 TSV 샘플 파일 존재
.tsv 파일의 구조:
1~3행: 시트 메타 정보 (무시됨)
4행 이후: 데이터 행
샘플 파일 MasterData - SheetInfo.tsv는 각 시트의 URL과 GID를 포함

SheetInfo를 읽어들일때 내부적으로 
SpreadSheetID
SheetID 을 url로 부터 parsing하여 저장합니다

Google Spreadsheet URL 형식:
```bash
https://docs.google.com/spreadsheets/d/{SpreadSheetID}/edit?gid={SheetID}
```

## 📄 주요 클래스 설명

| 클래스명               | 설명                                                                 |
|------------------------|----------------------------------------------------------------------|
| **BaseSheetConfigSO**  | 시트 URL과 시트 이름을 관리하는 설정 클래스                          |
| **BaseSheetDownloader**| 등록된 URL에서 CSV 데이터를 다운로드하고 SO로 저장                    |
| **BaseSheetConfigSOWindow** | Editor 유틸리티 창 (데이터 초기화, 자동 등록 지원)             |
| **SheetData.cs**       | SheetInfo의 URL 데이터를 관리, exportURL을 관리                         |
| **BaseSO.cs**          | SO 데이터 관리용 베이스 클래스                                       |
| **BaseData.cs**        | 개별 데이터 파싱 담당                                                |

## 🔧 커스터마이징

템플릿 경로: /Assets/ScriptTemplates/81-Sheet2SO__New SheetConfigSO-SheetConfigSO.cs

```
생성되는 클래스:
YourSheetConfigSO (BaseSheetConfigSO 상속)
YourSheetConfigSOEditor (Editor 확장)
YourSheetDownloader (BaseSheetDownloader 상속)
```

⚠️ 생성된 스크립트는 반드시 Editor 폴더 내에 있어야 정상 동작합니다.

```bash
유니티 재실행 후 Assets > Create > Sheet2SO > New SheetConfigSO 메뉴로 사용 가능
```


## 🧪 사용 방법 요약
```
SO 클래스 정의: BaseSheetConfigSO 상속 클래스와 구조체 정의
SheetInfo.tsv 작성: 각 스프레드시트의 ID/GID 등록
SheetConfigSO 에셋 생성: 시트 설정 등록
BaseSheetDownloader 실행: CSV → SO 변환
게임 내 Resources에서 로딩: 구조화된 데이터 활용
```


## **📜 라이선스**  
이 프로젝트는 **MIT 라이선스** 하에 배포됩니다. 자세한 내용은 [LICENSE](LICENSE) 파일을 확인하세요.  

## **📌 추가 정보**  
- Unity **2020.3 이상**에서 정상적으로 동작합니다.  
- 문제가 발생하면 [이슈](https://github.com/doppleddiggong/UIExtendButton/issues)에 등록해 주세요. 
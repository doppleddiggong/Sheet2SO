# **UI Extend Button**  
*Unity의 UI 버튼을 확장하여 더욱 유연한 비주얼과 상호작용을 제공합니다.*  

![Image](https://github.com/user-attachments/assets/9bd8a5a2-0a01-4d0a-b837-95a290c12097)

## **📌 개요**  
`UI Extend Button`은 UGUI 버튼을 확장하여 다양한 버튼 상태를 **자동으로 관리**할 수 있는 패키지입니다.  

✅ **커스텀 버튼 비주얼** (호버, 클릭, 비활성화 등)  
✅ **자동 색상 조절 기능**  
✅ **가볍고 사용하기 쉬운 구성**  

이제 **`ColorBlock`을 직접 설정하지 않아도** 버튼의 상태별 색상을 편리하게 조정할 수 있습니다.  

---

## **📦 설치 방법**  

### **manifest.json을 이용한 설치**  
Unity 프로젝트의 `Packages/manifest.json` 파일에 다음 내용을 추가하세요.  
```json
"dependencies": {
  "com.dopple.uiextendbutton": "https://github.com/doppleddiggong/UIExtendButton.git"
}
```

### **Git URL을 이용한 설치**  
- Click **Window** > **Package Manager** to **open Package Manager UI.**
- Click **+** > **Add package from git URL**... and input the repository URL : [](https://github.com/doppleddiggong/UIExtendButton.git)
    
    [https://github.com/doppleddiggong/UIExtendButton.git](https://github.com/doppleddiggong/UIExtendButton.git)

![image.png](https://github.com/user-attachments/assets/a19a7528-aa17-4964-a7bf-c8727faa1d08)


---

## **🚀 사용 방법**  

### **1. `ExtendButtonVisualCtrl` 컴포넌트 추가**  
- UI 버튼(GameObject)에 `ExtendButtonVisualCtrl` 컴포넌트를 추가하세요.  
- 버튼과 관련된 그래픽 요소를 **자동으로 감지하여 설정**합니다.  

### **2. 버튼 색상 설정**  
다음 항목을 Inspector에서 조정할 수 있습니다.  
- **호버(hover) 상태 색상**  
- **클릭(pressed) 상태 색상**  
- **비활성화(disabled) 상태 투명도 조절**  
- **아이콘 및 텍스트 색상**  

### 3. HoverSound, ClickSound를 제어 
ExtendButton을 통해 제어할 수 있습니다

### 4. UIElementScaler를 통해 Scale애니메이션을 제어할 수 있습니다

### 5. 자세한 사용은 SampleScene과 영상을 확인해보세요. 을 확인해보세요.
[![Video](https://img.youtube.com/vi/x5TORVhY0CU/0.jpg)](https://www.youtube.com/watch?v=x5TORVhY0CU)

---

## **🎨 Inspector 기능**  
이 패키지는 **인스펙터에서 바로 설정 가능한 버튼**을 제공합니다.  
- **"Recommand Tint Color"** → 버튼의 추천 색상 자동 설정  
- **"Set Normal State"** → 버튼을 기본 상태로 리셋  

---

## **📜 라이선스**  
이 프로젝트는 **MIT 라이선스** 하에 배포됩니다. 자세한 내용은 [LICENSE](LICENSE) 파일을 확인하세요.  

---

## **🔗 관련 링크**  
- **GitHub 저장소**: [UIExtendButton](https://github.com/doppleddiggong/UIExtendButton)  

---

## **📌 추가 정보**  
- Unity **2020.3 이상**에서 정상적으로 동작합니다.  
- 문제가 발생하면 [이슈](https://github.com/doppleddiggong/UIExtendButton/issues)에 등록해 주세요. 

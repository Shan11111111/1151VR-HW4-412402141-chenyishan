# 1151VR-HW4-412402141-chenyishan
-----------------------------------------
# 1151VR-HW4-412402141-陳怡珊

## 1、專案截圖

<!-- 將圖片上傳至 GitHub 的 Screenshots 資料夾後，可使用以下格式：
![Unity 遊戲場景](Screenshots/scene.png)
![飛行開始介面](Screenshots/start.png)
![障礙物關卡](Screenshots/obstacles.png)
![成功與失敗介面](Screenshots/result.png)
![Android 測試畫面](Screenshots/android.png)
-->
<img width="865" height="451" alt="image" src="https://github.com/user-attachments/assets/7c81ac51-f1cd-4f62-b0ca-12a3047df909" />

## 2、GitHub 連結

https://github.com/Shan11111111/1151VR-HW4-412402141-chenyishan

## 3、YouTube 連結

https://youtu.be/PcpaEBnBl-k

## 4、說明製作流程和相關操作

### 4.1 建立 Unity 3D 場景

- 使用 **Unity 6.3 LTS** 建立 3D 專案並建立 `MainScene`。
- 建立 `Ground`（Plane）及 `RunWay`（Cube），設計前進方向與飛行路徑，並調整材質顏色。
- 設定主攝影機，加入 `CameraFollow.cs`，讓畫面跟隨飛機前進。
- 加入場景裝飾，如拱門 `Arch_01_free_obs`、錐筒 `Cone_free` 等模型。

### 4.2 製作飛機模型與操控

- 初期以 Capsule、Cube 組裝機身與機翼，驗證基本飛行控制。
- 匯入正式飛機 `fly_model`，以 `Airplane` 為控制主物件，將新模型作為視覺子物件並停用測試模型。
- 在 `Airplane` 加入 Rigidbody（關閉重力、使用 Kinematic）及 Box Collider。
- 由 `AirplaneController.cs` 控制飛機沿 Z 軸自動前進，使用 `Rigidbody.MovePosition()` 更新位置，使用 `Mathf.Clamp()` 限制範圍。
- 使用 `CameraFollow.cs` 讓攝影機跟隨飛機，避免飛機離開鏡頭。

**開發期間最後確認的飛行設定：**

| 參數 | 數值 |
|---|---|
| Forward Speed | 4 |
| Horizontal Speed | 6 |
| Vertical Speed | 5 |
| Min X / Max X | -15 / 20 |
| Min Y / Max Y | 0.5 / 35 |
| Max Z | 250 |

以上限制針對飛機中心點，實際碰撞還需考慮模型及 Collider 的尺寸。

### 4.3 設計障礙牆、檢查點與終點

- 先以左右障礙柱測試通過與碰撞，後來擴充為多道牆面障礙。
- 使用 Cube 組成左右牆及上方障礙，保留可通過的開口；場景建立 `Gate01`、`Gate02`、`Gate03`。
- 調整不同障礙高度，讓玩家必須上升、下降、左右閃避。
- 各組關卡配置 `Checkpoint`，透過 Box Collider 的 `Is Trigger` 與 `FlightZone.cs` 偵測通過事件。
- 設置 `FinishFlag` 及旗幟模型，規劃作為終點；遊戲目標設定為 3 個檢查點。
- 以 `FlightFeedback.cs` 管理撞擊、通過進度與成功／失敗狀態。最終是否必須通過旗幟才判定勝利，仍應以繳交版測試結果確認。

### 4.4 三種音效與事件回饋

| 音效 | 觸發時機 | 播放方式 |
|---|---|---|
| 飛行引擎聲 | 點擊開始飛行後 | Audio Source 循環播放 |
| 撞擊音效 | 飛機碰撞障礙物 | `PlayOneShot()`，停止飛行 |
| 成功音效 | 通過有效檢查點／成功事件 | `PlayOneShot()` |

由 `FlightFeedback.cs` 管理引擎與效果兩個 Audio Source。開發時曾遇到 Audio Source 沒有指定、成功區域未勾選 `Is Trigger`，修正後三種音效基本事件已能觸發。

### 4.5 製作開始、遊玩及結果介面

- 以 Canvas 建立 `StartPanel`，提供標題、操作說明和 `StartButton`。
- 玩家點擊開始後，`GameHUD.cs` 呼叫飛行啟動方法，隱藏開始畫面並顯示方向按鈕。
- `ScoreText` 顯示例如「通過：0 / 3」；`ResultPanel` 顯示「挑戰成功！」或「飛行失敗！」。
- 使用 `RestartButton` 重新載入目前場景，回到初始流程。
- 以 `FlightControls` 統一管理四個觸控按鈕，尚未開始與遊戲結束時隱藏，正式飛行時才顯示。
- 匯入 Noto Sans TC，建立 `NotoSansTC_SDF` TextMeshPro 字型，修正中文缺字問題；Canvas Scaler 使用 `Scale With Screen Size`。

### 4.6 相關操作方式

| 功能 | 電腦鍵盤 | 遊戲畫面／手機 |
|---|---|---|
| 左右移動 | A / D 或 ← / → | 左／右方向按鈕 |
| 上升、下降 | W / S 或 ↑ / ↓ | 上／下方向按鈕 |
| 開始飛行 | 點擊「開始飛行」 | 點擊「開始飛行」 |
| 重新挑戰 | 點擊「重新挑戰」 | 點擊「重新挑戰」 |
| 向前飛行 | 自動前進 | 自動前進 |

四個方向鍵透過 `TouchFlightButton.cs` 處理按下與放開，支援持續按住移動。

### 4.7 主要 C# 腳本

| 檔名 | 功能 |
|---|---|
| `AirplaneController.cs` | 飛行速度、方向輸入、邊界限制 |
| `CameraFollow.cs` | 攝影機跟隨飛機 |
| `FlightZone.cs` | 障礙、檢查點與終點的觸發判定 |
| `FlightFeedback.cs` | 飛行狀態、音效、碰撞與進度 |
| `GameHUD.cs` | 開始介面、方向按鈕、結果與重新挑戰 |
| `TouchFlightButton.cs` | 電腦滑鼠／手機觸控按鈕的持續控制 |

### 4.8 問題排除與 Android 匯出

- **Unity 中文無法顯示：** 預設 LiberationSans SDF 缺少中文字形，改用 Noto Sans TC 動態字型資產。
- **撞擊／通過沒有聲音：** 補上 Audio Source 參照，並確保觸發區域勾選 `Is Trigger`。
- **飛機超出畫面：** 加入攝影機跟隨。
- **Missing (Mono Script)：** 檢查 C# 編譯錯誤、類別與檔名一致性，並重新連接場景元件；避免刪除並重建同名檔案而改變 `.meta` 參照。
- **方向鍵一直顯示：** 將四個按鈕移到 `FlightControls` 群組，由 `GameHUD` 管理顯示時機。
- **高度不足：** 因應高低不同的障礙，調整最高飛行高度為 35。
- **Android 建置路徑限制：** 切換 Android Build Profile 時，曾因專案路徑包含中文字而出現 `Invalid project path`；將專案改至全英文／數字路徑後重新建置。

**Android 建置狀態：成功。**

# Habit Tracker API

## 專案簡介

- 提供習慣管理、打卡、行為分析與報表功能的後端服務  
- 技術棧：ASP.NET Core 8、SQL Server、Dapper、Swagger、Hangfire（背景排程）  
- 採用 JWT 身份驗證，保障 API 安全，支援第三方串接  

---

## 功能簡介

- **習慣管理 CRUD**  
- **打卡功能**  
- **行為風格分析**  
- **週報與成長趨勢報表**  
- **提醒優先排序**  
- **系統日誌 (Audit Trail) 功能**：操作行為紀錄與管理  
- **Hangfire 背景排程**：自動清理與維護日誌資料  
- **JWT 認證保護**：API 授權安全控制  
- **Swagger UI 支援帶 Token 測試**  

---

## 技術架構

- ASP.NET Core 8 Web API  
- SQL Server 與 Stored Procedure  
- Dapper 輕量 ORM  
- Swagger UI 自動產生 API 文件  
- Hangfire 用於背景工作排程  
- 分層架構設計  
- JWT Bearer Token 身份驗證  

---

### 資料流程圖

```mermaid
graph TD
  User[使用者]
  API[API 接收請求]
  JWT[JWT 認證（授權檢查）]
  Controller[Controller（邏輯入口）]
  Service[Service / Repository（業務邏輯與資料存取）]
  DB[資料庫 (SQL Server)]
  Response[回傳結果給使用者]

  User --> API
  API --> JWT
  JWT --> Controller
  Controller --> Service
  Service --> DB
  DB --> Service
  Service --> Controller
  Controller --> Response
```
---

## API 文件與測試

- 啟動專案後，透過 Swagger UI 測試：  
  `https://localhost:{port}/swagger/index.html`  
- 使用 Swagger UI 頁面右上「Authorize」按鈕帶入 JWT Token  
- 先呼叫 `/api/auth/login` 取得 Token，再測試受保護 API  

---

## 環境設定與啟動

1. 安裝 .NET 8 SDK  
2. 安裝並設定 SQL Server  
3. 設定 `appsettings.json` 連線字串及 JWT 設定  
4. 啟動 API 專案  
5. 使用 Swagger 或 Postman 測試 API  

---

## 重要 API 範例

- `POST /api/auth/login`：使用者登入取得 JWT Token  
- `POST /api/habit`：建立習慣（需帶 Token）  
- `POST /api/habit/{id}/track`：打卡習慣（需帶 Token）  
- `GET /api/reports/weekly`：取得週報（需帶 Token）  
- `GET /api/reports/growth-trend`：取得成長趨勢（需帶 Token）  
- `GET /api/analysis/style`：行為風格分析（需帶 Token）  
- **所有重要操作均有系統日誌紀錄**  

---

## 未來擴充方向

- 推播通知整合  
- 使用者偏好提醒設定  
- 打卡異常偵測  
- 社群共習功能  
- API 版本管理完善  
- 日誌查詢與管理介面  

---

## 聯絡方式

- 作者：cafelee  
- Email：cafe307187749@gmail.com

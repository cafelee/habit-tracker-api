# Habit Tracker API

## 專案簡介

- 提供習慣管理、打卡、行為分析與報表功能的後端服務  
- 技術棧：ASP.NET Core、SQL Server、Dapper、Swagger  

---

## 功能簡介

- **習慣管理 CRUD**  
- **打卡功能**  
- **行為風格分析**  
- **週報與成長趨勢報表**  
- **提醒優先排序**  

---

## 技術架構

- ASP.NET Core 8 Web API  
- SQL Server 與 Stored Procedure  
- Dapper 輕量 ORM  
- Swagger UI  
- 分層架構設計  

---

## API 文件與測試

- 啟動專案後，透過 Swagger UI 測試：  
  `https://localhost:{port}/swagger/index.html`  

---

## 環境設定與啟動

1. 安裝 .NET 8 SDK  
2. 安裝並設定 SQL Server  
3. 設定 `appsettings.json` 連線字串  
4. 啟動 API 專案  
5. 使用 Swagger 或 Postman 測試 API  

---

## 重要 API 範例

- `POST /api/habit`：建立習慣  
- `POST /api/habit/{id}/track`：打卡習慣  
- `GET /api/reports/weekly`：取得週報  
- `GET /api/reports/growth-trend`：取得成長趨勢  
- `GET /api/analysis/style`：行為風格分析  

---

## 未來擴充方向

- 推播通知整合  
- 使用者偏好提醒設定  
- 打卡異常偵測  
- 社群共習功能  

---

## 聯絡方式

- 作者：cafelee  
- Email：cafe307187749@gmail.com


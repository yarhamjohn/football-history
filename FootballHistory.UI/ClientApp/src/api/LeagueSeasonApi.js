const baseUrlDev = "https://localhost:5001";
const baseUrlDevVS = "https://localhost:44338";
const baseUrl = process.env.NODE_ENV === "development" ? process.env.REACT_APP_API_URL : "https://footballhistoryapi.azurewebsites.net";

export default baseUrl;

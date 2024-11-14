import axiosInstance from "@/lib/common/apiHttp";
import type { ApiResponse } from "@/lib/type/typeApi";

/**
 * Standard Request Object
 */
class Req {
  code: number;
  meta: string;
  data: ReqData;
  constructor(data: string | object) {
    this.code = 0;
    this.meta = "";
    this.data = data;
  }
}

type ReqData = string | object;

/**
 * Get Method
 * @param url
 */
async function apiGet<T>(url: string): Promise<ApiResponse<T>> {
  const response = await axiosInstance.get(url);
  const responseData = response.data;
  responseData.status = {
    responseCode: response.status,
    responseText: response.statusText,
  };
  return responseData as ApiResponse<T>;
}

async function apiGetStr(url: string): Promise<ApiResponse<string>> {
  return await apiGet<string>(url);
}

/**
 * Post Method
 * @param url
 * @param data
 */
async function apiPost<T>(url: string, data: ReqData): Promise<ApiResponse<T>> {
  const response = await axiosInstance.post(url, new Req(data));
  const responseData = response.data;
  responseData.status = {
    responseCode: response.status,
    responseText: response.statusText,
  };
  return responseData as ApiResponse<T>;
}

/**
 * Post Method with string response
 * @param url
 * @param data
 */
async function apiPostStr(url: string, data: ReqData): Promise<ApiResponse<string>> {
  return await apiPost<string>(url, data);
}

/**
 * Post Method with bool response
 * @param url
 * @param data
 */
async function apiPostState(url: string, data: ReqData): Promise<ApiResponse<boolean>> {
  return await apiPost<boolean>(url, data);
}

/**
 * Put Method
 * @param url
 * @param data
 */
async function apiPut<T>(url: string, data: ReqData): Promise<ApiResponse<T>> {
  const response = await axiosInstance.put(url, new Req(data));
  const responseData = response.data;
  responseData.status = {
    responseCode: response.status,
    responseText: response.statusText,
  };
  return responseData as ApiResponse<T>;
}

/**
 * Put Method with bool response
 * @param url
 * @param data
 */
async function apiPutState(url: string, data: ReqData): Promise<ApiResponse<boolean>> {
  return await apiPut<boolean>(url, data);
}

/**
 * Delete Method
 * @param url
 */
async function apiDelete<T>(url: string): Promise<ApiResponse<T>> {
  const response = await axiosInstance.delete(url);
  const responseData = response.data;
  responseData.status = {
    responseCode: response.status,
    responseText: response.statusText,
  };
  return responseData as ApiResponse<T>;
}

async function getVersion(): Promise<number> {
  return (await axiosInstance.get("/version.json")).data.compileTime as number;
}

export { apiDelete, apiGet, apiPost, apiPut };

export { apiGetStr, apiPostState, apiPostStr, apiPutState };

export { getVersion };

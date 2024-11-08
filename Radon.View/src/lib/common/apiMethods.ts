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
  return (await axiosInstance.get(url)).data as ApiResponse<T>;
}

/**
 * Post Method
 * @param url
 * @param data
 */
async function apiPost<T>(url: string, data: ReqData): Promise<ApiResponse<T>> {
  return (await axiosInstance.post(url, new Req(data))).data as ApiResponse<T>;
}

/**
 * Post Method with string response
 * @param url
 * @param data
 */
async function apiPostStr(url: string, data: ReqData): Promise<ApiResponse<string>> {
  return (await axiosInstance.post(url, new Req(data))).data as ApiResponse<string>;
}

/**
 * Post Method with bool response
 * @param url
 * @param data
 */
async function apiPostState(url: string, data: ReqData): Promise<ApiResponse<boolean>> {
  return (await axiosInstance.post(url, new Req(data))).data as ApiResponse<boolean>;
}

/**
 * Put Method
 * @param url
 * @param data
 */
async function apiPut<T>(url: string, data: ReqData): Promise<ApiResponse<T>> {
  return (await axiosInstance.put(url, new Req(data))).data as ApiResponse<T>;
}

/**
 * Put Method with bool response
 * @param url
 * @param data
 */
async function apiPutState(url: string, data: ReqData): Promise<ApiResponse<boolean>> {
  return (await axiosInstance.put(url, new Req(data))).data as ApiResponse<boolean>;
}

/**
 * Delete Method
 * @param url
 */
async function apiDelete<T>(url: string): Promise<ApiResponse<T>> {
  return (await axiosInstance.delete(url)).data as ApiResponse<T>;
}

async function getVersion(): Promise<number> {
  return (await axiosInstance.get("/version.json")).data.compileTime as number;
}

export { apiDelete, apiGet, apiPost, apiPut };

export { apiPostState, apiPostStr, apiPutState };

export { getVersion };

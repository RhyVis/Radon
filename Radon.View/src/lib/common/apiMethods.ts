import axiosInstance from "@/lib/common/apiHttp";
import type { ApiResponse } from "@/lib/type/typeApi";

/**
 * Standard Request Object
 */
class Req {
  code: number;
  meta: string;
  data: string | object;
  constructor(data: string | object) {
    this.code = 0;
    this.meta = "";
    this.data = data;
  }
}

type CompileTime = {
  compileTime: number;
};

/**
 * Get Method
 * @param url
 * @param config
 */
async function apiGet<T>(url: string, config?: never): Promise<ApiResponse<T>> {
  return (await axiosInstance.get(url, config)).data as ApiResponse<T>;
}

/**
 * Post Method
 * @param url
 * @param data
 */
async function apiPost<T>(url: string, data: string | object): Promise<ApiResponse<T>> {
  return (await axiosInstance.post(url, new Req(data))).data as ApiResponse<T>;
}

/**
 * Post Method with string response
 * @param url
 * @param data
 */
async function apiPostStr(url: string, data: string | object): Promise<ApiResponse<string>> {
  return (await axiosInstance.post(url, new Req(data))).data as ApiResponse<string>;
}

/**
 * Post Method with bool response
 * @param url
 * @param data
 */
async function apiPostState(url: string, data: string | object): Promise<ApiResponse<boolean>> {
  return (await axiosInstance.post(url, new Req(data))).data as ApiResponse<boolean>;
}

/**
 * Put Method
 * @param url
 * @param data
 */
async function apiPut<T>(url: string, data: string | object): Promise<ApiResponse<T>> {
  return (await axiosInstance.put(url, new Req(data))).data as ApiResponse<T>;
}

/**
 * Put Method with bool response
 * @param url
 * @param data
 */
async function apiPutState(url: string, data: string | object): Promise<ApiResponse<boolean>> {
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
  return ((await axiosInstance.get("/version.json")).data as CompileTime).compileTime;
}

export { apiDelete, apiGet, apiPost, apiPut };

export { apiPostState, apiPostStr, apiPutState };

export { getVersion };

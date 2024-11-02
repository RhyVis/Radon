import type { ApiResponse, CompileTime } from "@/lib/type/typeApi";
import instance from "@/lib/util/apiHttp";

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

/**
 * Get Method
 * @param url
 * @param config
 */
async function apiGet(url: string, config?: never): Promise<ApiResponse> {
  return (await instance.get(url, config)).data as ApiResponse;
}

/**
 * Post Method
 * @param url
 * @param data
 */
async function apiPost(url: string, data: string | object): Promise<ApiResponse> {
  return (await instance.post(url, new Req(data))).data as ApiResponse;
}

/**
 * Put Method
 * @param url
 * @param data
 */
async function apiPut(url: string, data: string | object): Promise<ApiResponse> {
  return (await instance.put(url, new Req(data))).data as ApiResponse;
}

/**
 * Delete Method
 * @param url
 */
async function apiDelete(url: string): Promise<ApiResponse> {
  return (await instance.delete(url)).data as ApiResponse;
}

async function getVersion(): Promise<number> {
  return ((await instance.get("/version.json")).data as CompileTime).compileTime;
}

export { apiDelete, apiGet, apiPost, apiPut, getVersion };

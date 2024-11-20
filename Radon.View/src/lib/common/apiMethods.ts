import axiosInstance from "@/lib/common/apiHttp";
import type { AxiosRequestConfig } from "axios";

/**
 * Standard Request Object
 */
class Req {
  code: number;
  meta: string;
  data: ReqData;
  constructor(data: string | object | undefined) {
    this.code = 0;
    this.meta = "";
    this.data = data ?? {};
  }
}

type ReqData = string | object;

/**
 * @description Refer to ApiResBase defined in core
 */
type ApiResponse<T> = ApiResponseData<T> & ApiResponseStatus;

type ApiResponseData<T> = {
  code: number;
  msg: string;
  data: T;
};

type ApiResponseStatus = {
  status: {
    responseCode: number;
    responseText: string;
  };
};

/**
 * @description Refer to ExceptionRes defined in core, data is JSON serialized exception object
 */
type ErrResponse = ApiResponse<string>;

/**
 * Get Method
 * @param url
 * @param config
 */
async function apiGet<T>(url: string, config?: AxiosRequestConfig): Promise<ApiResponse<T>> {
  const response = await axiosInstance.get(url, config);
  let responseData: ApiResponse<T> = response.data;

  if (typeof responseData !== "object") {
    responseData = {
      code: 0,
      msg: "",
      data: {} as T,
      status: {
        responseCode: response.status,
        responseText: response.statusText,
      },
    };
  } else {
    responseData.status = {
      responseCode: response.status,
      responseText: response.statusText,
    };
  }

  return responseData;
}

async function apiGetStr(url: string, config?: AxiosRequestConfig): Promise<ApiResponse<string>> {
  return await apiGet<string>(url, config);
}

/**
 * Post Method
 * @param url
 * @param data
 * @param config
 */
async function apiPost<T>(url: string, data?: ReqData, config?: AxiosRequestConfig): Promise<ApiResponse<T>> {
  const response = await axiosInstance.post(url, new Req(data), config);
  let responseData: ApiResponse<T> = response.data;

  if (typeof responseData !== "object") {
    responseData = {
      code: 0,
      msg: "",
      data: {} as T,
      status: {
        responseCode: response.status,
        responseText: response.statusText,
      },
    };
  } else {
    responseData.status = {
      responseCode: response.status,
      responseText: response.statusText,
    };
  }

  return responseData;
}

/**
 * Post Method with string response
 * @param url
 * @param data
 * @param config
 */
async function apiPostStr(url: string, data?: ReqData, config?: AxiosRequestConfig): Promise<ApiResponse<string>> {
  return await apiPost<string>(url, data, config);
}

/**
 * Post Method with bool response
 * @param url
 * @param data
 * @param config
 */
async function apiPostState(url: string, data?: ReqData, config?: AxiosRequestConfig): Promise<ApiResponse<boolean>> {
  return await apiPost<boolean>(url, data, config);
}

/**
 * Post Method with file send in form
 * @param url
 * @param blob file data
 */
async function apiPostWithFile<T>(url: string, blob: Blob): Promise<ApiResponse<T>> {
  const formData = new FormData();
  formData.append("file", blob);
  const response = await axiosInstance.post(url, formData, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  });
  const responseData = response.data;
  responseData.status = {
    responseCode: response.status,
    responseText: response.statusText,
  };
  return responseData as ApiResponse<T>;
}

/**
 * Put Method
 * @param url
 * @param data
 * @param config
 */
async function apiPut<T>(url: string, data: ReqData, config?: AxiosRequestConfig): Promise<ApiResponse<T>> {
  const response = await axiosInstance.put(url, new Req(data), config);
  let responseData: ApiResponse<T> = response.data;

  if (typeof responseData !== "object") {
    responseData = {
      code: 0,
      msg: "",
      data: {} as T,
      status: {
        responseCode: response.status,
        responseText: response.statusText,
      },
    };
  } else {
    responseData.status = {
      responseCode: response.status,
      responseText: response.statusText,
    };
  }

  return responseData;
}

/**
 * Put Method with bool response
 * @param url
 * @param data
 * @param config
 */
async function apiPutState(url: string, data: ReqData, config?: AxiosRequestConfig): Promise<ApiResponse<boolean>> {
  return await apiPut<boolean>(url, data, config);
}

/**
 * Delete Method
 * @param url
 * @param config
 */
async function apiDelete<T>(url: string, config?: AxiosRequestConfig): Promise<ApiResponse<T>> {
  const response = await axiosInstance.delete(url, config);
  let responseData: ApiResponse<T> = response.data;

  if (typeof responseData !== "object") {
    responseData = {
      code: 0,
      msg: "",
      data: {} as T,
      status: {
        responseCode: response.status,
        responseText: response.statusText,
      },
    };
  } else {
    responseData.status = {
      responseCode: response.status,
      responseText: response.statusText,
    };
  }

  return responseData;
}

async function getVersion(): Promise<number> {
  return (await axiosInstance.get("/version.json")).data.compileTime as number;
}

async function getServerVersion(): Promise<string> {
  return (await axiosInstance.get("/api")).data.version as string;
}

export { apiDelete, apiGet, apiPost, apiPut };

export { apiGetStr, apiPostState, apiPostStr, apiPostWithFile, apiPutState };

export { getServerVersion, getVersion };

export type { ApiResponse, ErrResponse };

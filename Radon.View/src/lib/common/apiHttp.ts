import type { AxiosError, AxiosRequestConfig, AxiosResponse } from "axios";
import axios from "axios";

const axiosInstance = axios.create();

axiosInstance.defaults.baseURL = import.meta.env.VITE_API_ROOT;
axiosInstance.defaults.timeout = 10000;
axiosInstance.defaults.headers.post["Content-Type"] = "application/json";

axiosInstance.interceptors.request.use(
  // eslint-disable-next-line
  (config: AxiosRequestConfig): any => {
    config.headers!["Authorization"] = `Bearer ${localStorage.getItem("token")}`;
    return config;
  },
  (error: AxiosError) => {
    return Promise.reject(error);
  },
);

axiosInstance.interceptors.response.use(
  (response: AxiosResponse) => {
    // 对响应数据进行处理
    return response;
  },
  (error: AxiosError) => {
    // 处理响应错误
    return Promise.reject(error);
  },
);

export default axiosInstance;

import type { ErrResponse } from "@/lib/type/typeApi";
import type { AxiosError, AxiosRequestConfig, AxiosResponse } from "axios";
import axios from "axios";
import { MessagePlugin } from "tdesign-vue-next";

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
    return response;
  },
  async (error: AxiosError) => {
    const errData = (error.response?.data as ErrResponse) ?? { code: -1, msg: "Unknown Error", data: "" };
    await MessagePlugin.error(`ERR(${errData.code}): ${errData.msg}`);
    return Promise.reject(error);
  },
);

export default axiosInstance;

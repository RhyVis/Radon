import type { ErrResponse } from "@/lib/common/apiMethods";
import { getAuthToken } from "@/lib/common/authMethods.ts";
import i18n from "@/locale";
import type { AxiosError, AxiosRequestConfig, AxiosResponse } from "axios";
import axios from "axios";
import { MessagePlugin } from "tdesign-vue-next";

const { t } = i18n.global;

const respMsg = (resp: AxiosResponse) => {
  const k = `common.statusCode.${resp.status}`;
  const rr = t(k);
  return rr === k ? resp.statusText || t("common.unknownServerError") : rr;
};

const axiosInstance = axios.create();

axiosInstance.defaults.baseURL = import.meta.env.VITE_API_ROOT;
axiosInstance.defaults.timeout = 10000;
axiosInstance.defaults.headers.post["Content-Type"] = "application/json";

axiosInstance.interceptors.request.use(
  // eslint-disable-next-line
  (config: AxiosRequestConfig): any => {
    config.headers!["Authorization"] = `Bearer ${getAuthToken()}`;
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
    const resp = error.response;
    if (resp) {
      const errData = resp.data as ErrResponse;
      if (errData) {
        void MessagePlugin.error(`${t("common.serverError")}(${errData.code}): ${errData.msg}`);
        console.error(error);
        console.error(JSON.parse(errData.data));
      } else {
        void MessagePlugin.error(`${t("common.networkError")}(${resp.status}): ${respMsg(resp)}`);
      }
    } else {
      void MessagePlugin.error(t("common.networkError"));
    }

    return Promise.reject(error);
  },
);

export default axiosInstance;

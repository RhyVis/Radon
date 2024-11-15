import axiosInstance from "@/lib/common/apiHttp";
import { MessagePlugin } from "tdesign-vue-next";

type UsernamePair = {
  username: string;
  password: string;
};

/**
 * Login with the given username and password, token is stored in localStorage.
 * @param pair The username and password pair.
 */
async function authLogin(pair: UsernamePair): Promise<boolean> {
  try {
    const { code, data } = await apiPostStr("/api/auth/login", pair);
    if (code === 200) {
      localStorage.setItem("token", data);
      return true;
    } else {
      return false;
    }
  } catch (e) {
    console.warn(e);
    return false;
  }
}

/**
 * Logout, clear the token stored in localStorage.
 */
async function authLogout() {
  try {
    const response = await apiPost("/api/auth/logout");
    localStorage.removeItem("token");
    return response.status.responseCode === 204;
  } catch (e) {
    console.warn(e);
    return false;
  }
}

/**
 * Validate the token stored in localStorage.
 */
async function authValidate(): Promise<boolean> {
  const token = localStorage.getItem("token");
  if (token == null || token.length === 0) {
    await MessagePlugin.warning("令牌不存在");
    return false;
  }
  try {
    const { status } = await axiosInstance.post("/api/auth/validate");
    return status === 204;
  } catch (e) {
    console.warn(e);
    return false;
  }
}

/**
 * Validate the token stored in localStorage, if it is valid, refresh it.
 */
async function authValidateWithRefresh(): Promise<boolean> {
  if (await authValidate()) {
    return authRefresh().then(() => true);
  } else {
    return false;
  }
}

/**
 * Refresh the token stored in localStorage.
 */
async function authRefresh(): Promise<boolean> {
  try {
    const { data } = await apiPostStr("/api/auth/refresh");
    if (data != null && data.length > 0) {
      localStorage.setItem("token", data);
      return true;
    } else {
      return false;
    }
  } catch (e) {
    console.warn(e);
    return false;
  }
}

export { authLogin, authLogout, authRefresh, authValidate, authValidateWithRefresh };

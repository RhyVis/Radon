import axiosInstance from "@/lib/common/apiHttp";
import { apiPost } from "@/lib/common/apiMethods";

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
    const resp = await apiPost("/api/auth/login", pair);
    if (resp.code === 100) {
      localStorage.setItem("token", resp.data as string);
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
 * Validate the token stored in localStorage.
 */
async function authValidate(): Promise<boolean> {
  try {
    const { status } = await axiosInstance.post("/api/auth/validate", { data: localStorage.getItem("token") });
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
    void authRefresh();
    return true;
  } else {
    return false;
  }
}

/**
 * Refresh the token stored in localStorage.
 */
async function authRefresh(): Promise<boolean> {
  try {
    const resp = (await apiPost("/api/auth/refresh", localStorage.getItem("token") as string)).data as string;
    localStorage.setItem("token", resp);
    return true;
  } catch (e) {
    console.warn(e);
    return false;
  }
}

export { authLogin, authRefresh, authValidate, authValidateWithRefresh };

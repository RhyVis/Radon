import axiosInstance from "@/lib/common/apiHttp";

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
    if (code === 100) {
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
    const { data } = await apiPostStr("/api/auth/refresh", localStorage.getItem("token") ?? "");
    localStorage.setItem("token", data);
    return true;
  } catch (e) {
    console.warn(e);
    return false;
  }
}

export { authLogin, authRefresh, authValidate, authValidateWithRefresh };

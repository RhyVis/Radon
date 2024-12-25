import axiosInstance from "@/lib/common/apiHttp";
import i18n from "@/locale";
import { MessagePlugin } from "tdesign-vue-next";
import { apiPost } from "./apiMethods";

const { t } = i18n.global;

type UsernamePair = {
  username: string;
  password: string;
};

/**
 * Passport contains the token and user ID, with extra information.
 */
type Passport = {
  // Token used to interact with the server.
  token: string;
  // User ID
  userId: number;
  // Extra information
  extra: PassportExtra;
};

type PassportExtra = {
  // Image upload token
  imageToken: string;
  [key: string]: unknown;
};

const STORAGE_PASSPORT_KEY = "radon_passport";
const STORAGE_TOKEN_KEY = "radon_token";

/**
 * Get the token stored in localStorage.
 */
export function updateTokenInfo(passport?: Passport) {
  if (passport == null) {
    localStorage.removeItem(STORAGE_PASSPORT_KEY);
    localStorage.removeItem(STORAGE_TOKEN_KEY);
  } else {
    localStorage.setItem(STORAGE_PASSPORT_KEY, JSON.stringify(passport));
    localStorage.setItem(STORAGE_TOKEN_KEY, passport.token);
  }
}

export function getAuthToken(): string | null {
  return localStorage.getItem(STORAGE_TOKEN_KEY);
}

export function getAuthPassport(): Passport | null {
  const passport = localStorage.getItem(STORAGE_PASSPORT_KEY);
  if (passport != null && passport.length > 0) {
    return JSON.parse(passport) as Passport;
  } else {
    return null;
  }
}

/**
 * Login with the given username and password, token is stored in localStorage.
 * @param pair The username and password pair.
 */
export async function authLogin(pair: UsernamePair): Promise<boolean> {
  try {
    const { status, data } = await apiPost<Passport>("/api/auth/login", pair);
    if (status.responseCode === 200) {
      updateTokenInfo(data);
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
export async function authLogout(): Promise<boolean> {
  try {
    const response = await apiPost("/api/auth/logout");
    updateTokenInfo();
    return response.status.responseCode === 204;
  } catch (e) {
    console.warn(e);
    return false;
  }
}

/**
 * Validate the token stored in localStorage.
 */
export async function authValidate(): Promise<boolean> {
  const token = getAuthToken();
  if (token == null || token.length === 0 || token.trim().length === 0) {
    await MessagePlugin.warning(t("auth.tokenNotExists"));
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
export async function authValidateWithRefresh(): Promise<boolean> {
  if (await authValidate()) {
    return authRefresh().then(() => true);
  } else {
    return false;
  }
}

/**
 * Refresh the token stored in localStorage.
 */
export async function authRefresh(): Promise<boolean> {
  try {
    const { data } = await apiPost<Passport>("/api/auth/refresh");
    if (data != null) {
      updateTokenInfo(data);
      return true;
    } else {
      return false;
    }
  } catch (e) {
    console.warn(e);
    return false;
  }
}

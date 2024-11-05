import { apiPost } from "@/lib/util/apiMethods";

type UsernamePair = {
  username: string;
  password: string;
};

async function login(pair: UsernamePair): Promise<boolean> {
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

async function validate(token: string): Promise<boolean> {
  try {
    const resp = await apiPost("/api/auth/validate", token);
    return resp.code === 100;
  } catch (e) {
    console.warn(e);
    return false;
  }
}

export { login, validate };

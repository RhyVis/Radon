import { apiPost } from "@/lib/util/apiMethods";
import CryptoJS from "crypto-js";

const validateToken = async (token: string) => {
  if (token.length > 0) {
    try {
      const hashCode = CryptoJS.SHA256(token).toString(CryptoJS.enc.Hex);
      return ((await apiPost("api/auth", hashCode)).data as number) === 0;
    } catch (error) {
      console.error(error);
      return false;
    }
  } else {
    return false;
  }
};

export { validateToken };

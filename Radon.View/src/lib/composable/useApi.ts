import type { ApiResponse } from "@/lib/type/typeApi";

export const useApiStr = (url: string) => useApi<string>(url);

export function useApi<T>(url: string) {
  const result = ref<ApiResponse<T>>({
    code: 0,
    data: {} as T,
    msg: "",
  });

  const get = () => {
    return new Promise((resolve, reject) => {
      try {
        apiGet<T>(url).then(res => {
          result.value = res;
          resolve(res);
        });
      } catch (e) {
        console.error(e);
        reject(e);
      }
    });
  };

  const post = (data: unknown) => {
    return new Promise((resolve, reject) => {
      try {
        apiPost<T>(url, { data: data }).then(res => {
          result.value = res;
          resolve(res);
        });
      } catch (e) {
        console.error(e);
        reject(e);
      }
    });
  };

  return { result, get, post };
}

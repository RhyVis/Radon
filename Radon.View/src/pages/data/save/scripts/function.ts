import { apiDelete, apiGet, apiPutState } from "@/lib/common/apiMethods.ts";
import type { SaveBrief, SaveEntry } from "@/pages/data/save/scripts/define.ts";

export async function textStoreQuery(id: number) {
  const { data, code } = await apiGet<SaveEntry>(`/api/text-store/${id}`, { timeout: 30000 });
  return { data, code };
}

export async function textStoreQueryAll() {
  const { data, code } = await apiGet<SaveBrief[]>("/api/text-store", { timeout: 120000 });
  return { data, code };
}

export async function textStoreUpdate(data: unknown) {
  return await apiPutState("/api/text-store", data, { timeout: 30000 });
}

export async function textStoreDelete(id: number) {
  return await apiDelete<boolean>(`/api/text-store/${id}`, { timeout: 10000 });
}

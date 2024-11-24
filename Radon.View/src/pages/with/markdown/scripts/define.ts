import { apiGet, apiPut } from "@/lib/common/apiMethods.ts";

export type MdIndex = {
  path: string;
  name: string;
  desc: string;
};

export type MdRecord = {
  name: string;
  content: string;
};

export const getMdIndex = async (): Promise<MdIndex[]> => {
  const index = await apiGet<MdIndex[]>("/api/md/index");
  return index.data;
};

export const getMdRecord = async (path: string): Promise<MdRecord> => {
  const { data } = await apiGet<MdRecord>(`/api/md/${path}`);
  return data;
};

export const updateMdRecord = async (path: string, name: string, desc: string, content: string): Promise<string> => {
  const res = await apiPut<string>(`api/md/${path}`, { path, name, desc, content });
  return res.data;
};

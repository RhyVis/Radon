import { apiDeleteNoContent, apiGet, apiPostStr, apiPut } from "@/lib/common/apiMethods.ts";

export type MdIndexDto = {
  path: string;
  name: string;
  desc: string;
  createTime: string;
  updateTime: string;
};

export type MdIndex = MdIndexDto & { content: string };

export type MdRecord = {
  name: string;
  desc: string;
  content: string;
};

export const getMdIndex = async (): Promise<MdIndexDto[]> => {
  const index = await apiGet<MdIndexDto[]>("/api/archive/index", { timeout: 20000 });
  return index.data;
};

export const getMdRecord = async (path: string): Promise<MdRecord> => {
  const { data } = await apiGet<MdRecord>(`/api/archive/${path}`);
  return data;
};

export const checkMdRecord = async (path: string): Promise<MdIndex> => {
  const { data } = await apiGet<MdIndex>(`/api/archive/check/${path}`);
  return data;
};

export const updateMdRecord = async (path: string, name: string, desc: string, content: string): Promise<string> => {
  const res = await apiPut<string>(`api/archive/${path}`, {
    path: path,
    name: name,
    desc: desc,
    content: content,
  });
  if (res.data.length > 0) console.warn("Path unexpectedly changed to", res.data);
  return res.data;
};

export const createMdRecord = async (name: string, desc: string, content: string): Promise<string> => {
  const res = await apiPostStr(`api/archive/create`, {
    path: "",
    name: name,
    desc: desc,
    content: content,
  });
  return res.data;
};

export const deleteMdRecord = async (path: string): Promise<void> => {
  await apiDeleteNoContent(`api/archive/${path}`);
};

import { renderPdxColor, replacePdxAlias } from "@/pages/with/pdx-parser/scripts/function.ts";
import { usePdxStore } from "@/pages/with/pdx-parser/scripts/store.ts";
import { get, useDark } from "@vueuse/core";
import { storeToRefs } from "pinia";

export const usePdxTextRender = () => {
  const { replacer } = storeToRefs(usePdxStore());
  const dark = useDark();

  const textRender = (raw: string) => renderPdxColor(raw, get(replacer), get(dark));
  const textAlias = (raw: string) => replacePdxAlias(raw, get(replacer));

  return { textRender, textAlias };
};

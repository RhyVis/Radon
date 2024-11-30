import CharaList from "@/assets/conf/arcaea-characters.json";
import type { CharacterDefinition } from "@/pages/draw/pjsk-sticker/scripts/define.ts";

export const arcaeaCharaList: CharacterDefinition[] = CharaList.map(chara => ({
  id: chara.id,
  name: chara.name,
  chara: chara.chara,
  color: chara.fillColor,
  img: chara.img,
  defaultText: chara.defaultText,
}));

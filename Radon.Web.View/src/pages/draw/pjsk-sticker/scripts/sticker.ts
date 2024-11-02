import CharacterList from "@/assets/conf/characters.json";
import type { CharacterDefinition } from "@/lib/type/typeSticker";

const charaList: CharacterDefinition[] = CharacterList;

const assembleSrc = (s: string) => import.meta.env.VITE_RES_ROOT + "/pjsk-sticker/" + s;

export { assembleSrc, charaList };

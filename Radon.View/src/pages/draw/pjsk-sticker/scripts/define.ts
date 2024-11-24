import CharacterList from "@/assets/conf/characters.json";

export type CharacterDefinition = {
  id: number;
  name: string;
  chara: string;
  img: string;
  color: string;
  defaultText: {
    text: string;
    x: number;
    y: number;
    r: number;
    s: number;
  };
};

export const charaList: CharacterDefinition[] = CharacterList;

export const assembleSrc = (s: string) => import.meta.env.VITE_RES_ROOT + "/pjsk-sticker/" + s;

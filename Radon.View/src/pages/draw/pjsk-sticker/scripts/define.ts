import CharacterList from "@/assets/conf/pjsk-characters.json";

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

export const pjskCharaList: CharacterDefinition[] = CharacterList;

export const assembleSrc = (s: string, endpoint: string) => `${import.meta.env.VITE_RES_ROOT}/${endpoint}/${s}`;

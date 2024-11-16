import type { Loader } from "@/lib/composable/useLoader";

type FontInfo = {
  name: string;
  url: string;
};

const fontList: FontInfo[] = [
  {
    name: "YurukaStd",
    url: "/font/YurukaStd.woff2",
  },
  {
    name: "SSFangTangTi",
    url: "/font/ShangShouFangTangTi.woff2",
  },
  {
    name: "LilitaOne",
    url: "/font/LilitaOne-Regular.ttf",
  },
  {
    name: "ChildFunSansFusion-Z",
    url: "/font/ChildFunSansFusion-Z.ttf",
  },
  {
    name: "RoGSanSrfStd-Bd",
    url: "/font/RoGSanSrfStd-Bd_other_mod.woff2",
  },
  {
    name: "RoGSanSrfStd-Bd",
    url: "/font/RoGSanSrfStd-Bd_CJK.woff2",
  },
  {
    name: "GlowSans",
    url: "/font/GlowSansSC-Normal-Heavy.otf",
  },
  {
    name: "IosevkaRx",
    url: "/font/IosevkaRx-Regular.woff2",
  },
  {
    name: "IosevkaRx",
    url: "/font/IosevkaRx-Bold.woff2",
  },
];

const base = import.meta.env.VITE_RES_ROOT;

const getFontLoaders = (): Loader[] => {
  return fontList.map(font => {
    return {
      name: font.name,
      action: new FontFace(font.name, `url(${base + font.url})`).load().then(loadedFont => {
        document.fonts.add(loadedFont);
        console.debug(`Successfully loaded ${font.name}`);
      }),
    };
  });
};

export { getFontLoaders };

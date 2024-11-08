const fontList = [
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

async function loadFonts() {
  try {
    const loadPromises = fontList.map(async font => {
      const fontFace = new FontFace(font.name, `url(${base + font.url})`);
      return fontFace.load().then(loadedFont => {
        document.fonts.add(loadedFont);
        console.debug(`Successfully loaded ${font.name}`);
      });
    });
    await Promise.all(loadPromises);
    console.log("Successfully loaded all fonts");
  } catch (e) {
    console.error("Failed to load fonts", e);
    throw e;
  }
}

export { loadFonts };

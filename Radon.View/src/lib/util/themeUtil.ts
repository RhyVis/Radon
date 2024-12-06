export const changeMetaColor = (color: string) => {
  const metaThemeColor = document.querySelector('meta[name="theme-color"]');
  if (metaThemeColor) {
    metaThemeColor.setAttribute("content", color);
  }
};

export const changeMetaColorDark = (dark: boolean) => {
  changeMetaColor(dark ? "#121212" : "#ffffff");
};

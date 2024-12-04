async function loadImage(url: string): Promise<HTMLImageElement> {
  return new Promise((resolve, reject) => {
    const img = new Image();
    img.crossOrigin = "anonymous";
    img.src = url;
    img.onload = () => resolve(img);
    img.onerror = reject;
  });
}

export async function loadAllBaseImg(): Promise<Record<string, HTMLImageElement>> {
  const baseUrl = import.meta.env.VITE_RES_ROOT;
  const halo = await loadImage(baseUrl + "/ba-banner/halo.png");
  const cross = await loadImage(baseUrl + "/ba-banner/cross.png");
  return {
    halo: halo,
    cross: cross,
  };
}

<script lang="ts" setup>
import type { DrawConf } from "@/lib/type/typeSticker";
import { assembleSrc, charaList } from "@/pages/draw/pjsk-sticker/scripts/sticker";

const props = defineProps<{
  conf: DrawConf;
}>();
const loading = ref(false);

const canvasRef = ref<HTMLCanvasElement>();
const imageRef = ref(new Image());
const fontFamily = computed(() =>
  props.conf.useCommercialFonts
    ? "YurukaStd, SSFangTangTi, LilitaOne, ChildFunSansFusion-Z"
    : "LilitaOne, ChildFunSansFusion-Z",
);

const draw = (conf: DrawConf) => {
  const { charaID, fontSize, spaceSize, rotate, x, y, text, curve } = conf;
  loading.value = true;

  const character = charaList[charaID];
  imageRef.value.crossOrigin = "anonymous";
  imageRef.value.src = assembleSrc(character.img);

  if (!canvasRef.value) return console.error("无法获取Canvas元素");
  const canvas = canvasRef.value;
  canvas.height = 296;
  canvas.width = 296;
  const ctx = canvas.getContext("2d");

  if (!ctx) return console.error("无法获取Context2D");
  imageRef.value.onload = () => {
    const img = imageRef.value;
    const hRatio = canvas.width / img.width;
    const vRatio = canvas.height / img.height;
    const aRatio = Math.min(hRatio, vRatio);
    const centerShiftX = (canvas.width - img.width * aRatio) / 2;
    const centerShiftY = (canvas.height - img.height * aRatio) / 2;
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    ctx.drawImage(
      img,
      0,
      0,
      img.width,
      img.height,
      centerShiftX,
      centerShiftY,
      img.width * aRatio,
      img.height * aRatio,
    );
    ctx.font = `${fontSize}px ${fontFamily.value}`;
    ctx.lineWidth = 9;
    ctx.save();

    ctx.translate(x, y);
    ctx.rotate(rotate / 10);
    ctx.textAlign = "center";
    ctx.strokeStyle = "white";
    ctx.fillStyle = character.color;

    const lines = text.split("\n");
    const angle = (Math.PI * text.length) / 7;
    if (curve) {
      for (const line of lines) {
        for (let i = 0; i < line.length; i++) {
          ctx.rotate(angle / line.length / 2.5);
          ctx.save();
          ctx.translate(0, -1 * fontSize * 3.5);
          ctx.strokeText(line[i], 0, 0);
          ctx.fillText(line[i], 0, 0);
          ctx.restore();
        }
      }
    } else {
      for (let i = 0, k = 0; i < lines.length; i++) {
        ctx.strokeText(lines[i], 0, k);
        ctx.fillText(lines[i], 0, k);
        k += spaceSize;
      }
      ctx.restore();
    }
    loading.value = false;
  };
};

onMounted(() => {
  draw(props.conf);
});
</script>

<template>
  <div id="sticker-container">
    <t-loading :loading="loading" size="small" :delay="50">
      <canvas id="sticker-canvas" ref="canvasRef" />
    </t-loading>
  </div>
</template>

<style scoped>
#sticker-canvas {
  border: 1px solid #bbb;
  border-radius: 8px;
}
</style>

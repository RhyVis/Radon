<script lang="ts" setup>
import { assembleSrc, charaList } from "@/pages/draw/pjsk-sticker/scripts/define";
import { usePjskStore } from "@/pages/draw/pjsk-sticker/scripts/store";
import { get, useToggle } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { computed, onMounted, ref } from "vue";

const store = usePjskStore();
const { charaId, fontSize, spaceSize, rotate, x, y, text, curve, useCommercialFonts } = storeToRefs(store);
const [loading, setLoading] = useToggle(false);

const canvasRef = ref<HTMLCanvasElement>();
const imageRef = ref(new Image());
const fontFamily = computed(() =>
  get(useCommercialFonts)
    ? "YurukaStd, SSFangTangTi, LilitaOne, ChildFunSansFusion-Z"
    : "LilitaOne, ChildFunSansFusion-Z",
);

const draw = () => {
  setLoading(true);

  const character = charaList[get(charaId)];
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
    ctx.font = `${get(fontSize)}px ${get(fontFamily)}`;
    ctx.lineWidth = 9;
    ctx.save();

    ctx.translate(get(x), get(y));
    ctx.rotate(get(rotate) / 10);
    ctx.textAlign = "center";
    ctx.strokeStyle = "white";
    ctx.fillStyle = character.color;

    const lines = get(text).split("\n");
    const angle = (Math.PI * text.value.length) / 7;
    if (get(curve)) {
      for (const line of lines) {
        for (let i = 0; i < line.length; i++) {
          ctx.rotate(angle / line.length / 2.5);
          ctx.save();
          ctx.translate(0, -1 * get(fontSize) * 3.5);
          ctx.strokeText(line[i], 0, 0);
          ctx.fillText(line[i], 0, 0);
          ctx.restore();
        }
      }
    } else {
      for (let i = 0, k = 0; i < lines.length; i++) {
        ctx.strokeText(lines[i], 0, k);
        ctx.fillText(lines[i], 0, k);
        k += get(spaceSize);
      }
      ctx.restore();
    }
    setLoading(false);
  };
};

onMounted(() => {
  draw();
});
store.$subscribe(() => {
  draw();
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

<script lang="ts" setup>
import { loadAllBaseImg } from "@/pages/draw/ba-banner/scripts/loadBaseImg";
import { computed, onMounted, ref } from "vue";

const {
  canvasWidth = 900,
  canvasHeight = 250,
  fontSize = 84,
  textBaseLine = 0.68,
  horizontalTilt = -0.4,
  paddingX = 10,
  graphOffsetX = -15,
  graphOffsetY = 0,
  textLeft = "Blue",
  textRight = "Archive",
  transparentBackground = false,
} = defineProps<{
  canvasWidth?: number;
  canvasHeight?: number;
  fontSize?: number;
  textBaseLine?: number;
  horizontalTilt?: number;
  paddingX?: number;
  graphOffsetX?: number;
  graphOffsetY?: number;
  textLeft?: string;
  textRight?: string;
  transparentBackground?: boolean;
}>();
const fontVal = computed(
  () =>
    `${fontSize}px RoGSanSrfStd-Bd, GlowSansSC, apple-system, BlinkMacSystemFont, Segoe UI,
    Helvetica, Arial, PingFang SC, Hiragino Sans GB, Microsoft YaHei, sans-serif, serif`,
);
const textMetricsL = ref<TextMetrics | null>();
const textMetricsR = ref<TextMetrics | null>();
const textWidthL = ref(0);
const textWidthR = ref(0);
const canvasWidthL = ref(0);
const canvasWidthR = ref(0);
const loading = ref(true);
const canvasRef = ref<HTMLCanvasElement>();

const hollowPath = [
  [284, 136],
  [321, 153],
  [159, 410],
  [148, 403],
];

canvasWidthL.value = canvasWidth / 2;
canvasWidthR.value = canvasWidth / 2;

const doDraw = async () => {
  // Context
  if (!canvasRef.value) return console.error("Can't find canvas");
  canvasRef.value.width = canvasWidth;
  canvasRef.value.height = canvasHeight;

  const ctx = canvasRef.value.getContext("2d");
  if (!ctx) return console.error("Can't find context");

  // Font & Text
  ctx.font = fontVal.value;
  textMetricsL.value = ctx.measureText(textLeft);
  textMetricsR.value = ctx.measureText(textRight);
  setWidth();

  ctx.clearRect(0, 0, canvasRef.value.width, canvasRef.value.height);

  // Background
  if (!transparentBackground) {
    ctx.fillStyle = "#fff";
    ctx.fillRect(0, 0, canvasRef.value.width, canvasRef.value.height);
  }

  ctx.font = fontVal.value;
  ctx.fillStyle = "#128AFA";
  ctx.textAlign = "end";
  ctx.setTransform(1, 0, horizontalTilt, 1, 0, 0);
  ctx.fillText(textLeft, canvasWidthL.value, canvasRef.value.height * textBaseLine);
  ctx.resetTransform();

  const baseImages = await loadAllBaseImg();
  ctx.drawImage(
    baseImages.halo,
    canvasWidthL.value - canvasRef.value.height / 2 + graphOffsetX,
    graphOffsetY,
    canvasHeight,
    canvasHeight,
  );
  ctx.fillStyle = "#2B2B2B";
  ctx.textAlign = "start";
  if (transparentBackground) {
    ctx.globalCompositeOperation = "destination-out";
  }
  ctx.strokeStyle = "white";
  ctx.lineWidth = 12;
  ctx.setTransform(1, 0, horizontalTilt, 1, 0, 0);
  ctx.strokeText(textRight, canvasWidthL.value, canvasRef.value.height * textBaseLine);
  ctx.globalCompositeOperation = "source-over";
  ctx.fillText(textRight, canvasWidthL.value, canvasRef.value.height * textBaseLine);
  ctx.resetTransform();

  const graph = {
    X: canvasWidthL.value - canvasRef.value.height / 2 + graphOffsetX,
    Y: graphOffsetY,
  };

  ctx.beginPath();
  ctx.moveTo(graph.X + (hollowPath[0][0] / 500) * canvasHeight, graph.Y + (hollowPath[0][1] / 500) * canvasHeight);
  for (let i = 1; i < 4; i++) {
    ctx.lineTo(graph.X + (hollowPath[i][0] / 500) * canvasHeight, graph.Y + (hollowPath[i][1] / 500) * canvasHeight);
  }
  ctx.closePath();
  if (transparentBackground) {
    ctx.globalCompositeOperation = "destination-out";
  }
  ctx.fillStyle = "white";
  ctx.fill();
  ctx.globalCompositeOperation = "source-over";
  ctx.drawImage(
    baseImages.cross,
    canvasWidthL.value - canvasRef.value.height / 2 + graphOffsetX,
    graphOffsetY,
    canvasHeight,
    canvasHeight,
  );
  loading.value = false;
  console.log("Draw Complete");
};
const setWidth = () => {
  textWidthL.value =
    textMetricsL.value!.width -
    (textBaseLine * canvasHeight + textMetricsL.value!.fontBoundingBoxDescent) * horizontalTilt;
  textWidthR.value =
    textMetricsR.value!.width -
    (textBaseLine * canvasHeight + textMetricsR.value!.fontBoundingBoxDescent) * horizontalTilt;

  if (textWidthL.value + paddingX > canvasWidth / 2) {
    canvasWidthL.value = textWidthL.value + paddingX;
  } else {
    canvasWidthL.value = canvasWidth / 2;
  }
  if (textWidthR.value + paddingX > canvasWidth / 2) {
    canvasWidthR.value = textWidthR.value + paddingX;
  } else {
    canvasWidthR.value = canvasWidth / 2;
  }
  canvasRef.value!.width = canvasWidthL.value + canvasWidthR.value;
};
const generateOutputImage = () => {
  return new Promise<Blob>((resolve, reject) => {
    canvasRef.value!.toBlob(blob => {
      if (blob) {
        resolve(blob);
      } else {
        reject();
      }
    });
  });
};

defineExpose({
  generateOutputImage,
});

onMounted(async () => {
  await doDraw();
});
</script>

<template>
  <div id="ba-banner-container">
    <t-loading :delay="100" :loading="loading" size="small">
      <canvas id="ba-banner" ref="canvasRef" />
    </t-loading>
  </div>
</template>

<style scoped>
#ba-banner {
  border: 1px solid #bbb;
  border-radius: 8px;
  max-width: 100%;
}
</style>

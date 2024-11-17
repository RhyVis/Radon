<script lang="ts" setup>
import { useKeyUpdate } from "@/composable/useKeyUpdate";
import { copyImage, downloadImage } from "@/lib/util/imageUtil";
import BannerCanvas from "@/pages/draw/ba-banner/comps/BannerCanvas.vue";
import { get } from "@vueuse/core";
import { CopyIcon, DownloadIcon, RefreshIcon } from "tdesign-icons-vue-next";
import { ref } from "vue";

const textL = ref("Blue");
const textR = ref("Archive");
const offsetX = ref(-15);
const offsetY = ref(0);
const tBg = ref(false);
const bannerCanvasRef = ref();
const { key, updateKey } = useKeyUpdate();

const handleCopyImage = async () => {
  const blob = await bannerCanvasRef.value.generateOutputImage();
  await copyImage(blob);
};
const handleDownloadImage = async () => {
  const blob = await bannerCanvasRef.value.generateOutputImage();
  await downloadImage(blob, `${get(textL) + get(textR)}-ba-banner.png`);
};
</script>

<template>
  <content-layout title="BA横幅" subtitle="移植自蔚蓝档案标题生成器">
    <div class="mb-3 mt-1 r-d-bb-canvas-container">
      <div>
        <BannerCanvas
          :key="key"
          :graph-offset-x="offsetX"
          :graph-offset-y="offsetY"
          :text-left="textL"
          :text-right="textR"
          :transparent-background="tBg"
          ref="bannerCanvasRef"
        />
      </div>
    </div>
    <t-form>
      <t-form-item label="左/右标题">
        <t-space align="center" direction="vertical">
          <t-input v-model="textL" @change="updateKey" />
          <t-input v-model="textR" @change="updateKey" />
        </t-space>
      </t-form-item>
      <t-form-item label="透明背景">
        <t-switch v-model="tBg" @change="updateKey" />
      </t-form-item>
      <t-form-item label="高级选项">
        <adv-opt-container>
          <t-tag class="r-no-select" content="光环位置" />
          <t-input-number v-model="offsetX" size="small" @change="updateKey" />
          <t-input-number v-model="offsetY" size="small" @change="updateKey" />
        </adv-opt-container>
      </t-form-item>
      <t-form-item label="立即重绘">
        <t-button shape="circle" @click="updateKey">
          <RefreshIcon />
        </t-button>
      </t-form-item>
      <t-form-item label="操作">
        <t-space :size="5">
          <t-button shape="square" theme="default" @click="handleCopyImage">
            <CopyIcon />
          </t-button>
          <t-button shape="square" theme="default" @click="handleDownloadImage">
            <DownloadIcon />
          </t-button>
        </t-space>
      </t-form-item>
    </t-form>
  </content-layout>
</template>

<style scoped>
.r-d-bb-canvas-container {
  text-align: center;
}
</style>

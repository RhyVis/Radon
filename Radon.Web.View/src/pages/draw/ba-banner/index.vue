<script lang="tsx" setup>
import ContentLayout from "@/layout/frame/ContentLayout.vue";
import BannerCanvas from "@/pages/draw/ba-banner/comps/BannerCanvas.vue";
import AdvancedOptContainer from "@/components/menu/AdvOptContainer.vue";
import { ref } from "vue";
import { CopyIcon, DownloadIcon, RefreshIcon } from "tdesign-icons-vue-next";
import { copyImage, downloadImage } from "@/lib/util/imageUtil";

const subtitle = () => {
  return (
    <t-space size={6} class="text-muted">
      <span>移植自</span>
      <t-link href="https://tmp.nulla.top/ba-logo/" suffix-icon={subtitleLinkIcon}>
        蔚蓝档案标题生成器
      </t-link>
    </t-space>
  );
};
const subtitleLinkIcon = () => <t-icon name="jump" />;

const canvasKey = ref(0);
const textL = ref("Blue");
const textR = ref("Archive");
const offsetX = ref(-15);
const offsetY = ref(0);
const tBg = ref(false);

const bannerCanvasRef = ref();

const handleUpdate = () => {
  canvasKey.value = new Date().getTime();
};
const handleCopyImage = async () => {
  const blob = await bannerCanvasRef.value.generateOutputImage();
  await copyImage(blob);
};
const handleDownloadImage = async () => {
  const blob = await bannerCanvasRef.value.generateOutputImage();
  await downloadImage(blob, `${textL.value + textR.value}-ba-banner.png`);
};
</script>

<template>
  <ContentLayout title="BA横幅" :subtitle="subtitle">
    <div class="mb-3 mt-1 r-d-bb-canvas-container">
      <div>
        <BannerCanvas
          :key="canvasKey"
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
          <t-input v-model="textL" @change="handleUpdate" />
          <t-input v-model="textR" @change="handleUpdate" />
        </t-space>
      </t-form-item>
      <t-form-item label="透明背景">
        <t-switch v-model="tBg" @change="handleUpdate" />
      </t-form-item>
      <t-form-item label="高级选项">
        <AdvancedOptContainer>
          <t-tag class="r-no-select" content="光环位置" />
          <t-input-number v-model="offsetX" size="small" @change="handleUpdate" />
          <t-input-number v-model="offsetY" size="small" @change="handleUpdate" />
        </AdvancedOptContainer>
      </t-form-item>
      <t-form-item label="立即重绘">
        <t-button shape="circle" @click="handleUpdate">
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
  </ContentLayout>
</template>

<style scoped>
.r-d-bb-canvas-container {
  text-align: center;
}
</style>

<script lang="tsx" setup>
import { CopyIcon, DownloadIcon } from "tdesign-icons-vue-next";
import { b64ToBlob } from "@/lib/util/imageTransform";
import { usePjskStore } from "@/pages/draw/pjsk-sticker/scripts/store";
import CharacterList from "@/assets/conf/characters.json";
import StickerCanvas from "@/pages/draw/pjsk-sticker/comps/StickerCanvas.vue";
import SelectChara from "@/pages/draw/pjsk-sticker/comps/SelectChara.vue";
import type { CharacterDefinition, DrawConf } from "@/lib/type/typeSticker";
import { copyImage, downloadImage } from "@/lib/util/imageUtil";

const subtitle = () => {
  return (
    <t-space size={6} class="text-muted">
      <span>移植自</span>
      <t-link href="https://st.ayaka.one/" suffix-icon={subtitleLinkIcon}>
        Project Sekai Stickers
      </t-link>
    </t-space>
  );
};
const subtitleLinkIcon = () => <t-icon name="jump" />;

const charaList = CharacterList as CharacterDefinition[];

const store = usePjskStore();

const currentConf = reactive<DrawConf>({
  charaID: 0,
  fontSize: 1,
  spaceSize: 1,
  rotate: 0,
  x: 0,
  y: 0,
  text: "",
  curve: false,
  useCommercialFonts: true,
});
const currentConfYProxy = ref(360);

const stickerCanvasRef = ref();
const stickerCanvasKey = ref(0);

const textMultipleLines = computed(() => currentConf.text.includes("\n"));

const updateCurrentConf = (id: number) => {
  const chara = charaList[id];
  currentConf.fontSize = chara.defaultText.s;
  currentConf.rotate = chara.defaultText.r;
  currentConf.x = chara.defaultText.x;
  currentConf.y = chara.defaultText.y;
  currentConfYProxy.value = 360 - chara.defaultText.y;
  currentConf.text = chara.defaultText.text;
};

const proxyDraw = () => {
  store.charaId = currentConf.charaID;
  store.useCommercialFonts = currentConf.useCommercialFonts;
  stickerCanvasKey.value = new Date().getTime();
};

const handleText = () => {
  if (currentConf.text.includes("\n")) {
    const match = currentConf.text.match(/\n/g)?.length ?? 0;
    currentConf.spaceSize = 52 * match;
    proxyDraw();
  } else {
    proxyDraw();
  }
};
const handleSelect = (index: number) => {
  currentConf.charaID = index;
  updateCurrentConf(index);
  proxyDraw();
};
const handleYProxy = () => {
  currentConf.y = 360 - currentConfYProxy.value;
  proxyDraw();
};
const handleCopyImage = async () => {
  const canvas = document.getElementById("sticker-canvas") as HTMLCanvasElement;
  await copyImage(b64ToBlob(canvas?.toDataURL().split(",")[1]));
};
const handleDownloadImage = async () => {
  const canvas = document.getElementById("sticker-canvas") as HTMLCanvasElement;
  await downloadImage(canvas.toDataURL(), `${charaList[currentConf.charaID].name}_sticker.png`);
};

onMounted(() => {
  currentConf.charaID = store.charaId;
  currentConf.useCommercialFonts = store.useCommercialFonts;
  updateCurrentConf(currentConf.charaID);
});
</script>

<template>
  <content-layout title="PJSK表情" :subtitle="subtitle">
    <div class="mb-3 mt-1" style="text-align: center">
      <t-space :size="8" direction="vertical">
        <t-space :size="16" direction="horizontal">
          <div style="height: 300px; width: 300px">
            <StickerCanvas :key="stickerCanvasKey" :conf="currentConf" ref="stickerCanvasRef" />
          </div>
          <t-slider v-model="currentConfYProxy" :max="360" layout="vertical" @change="handleYProxy" />
        </t-space>
        <t-slider v-model="currentConf.x" :max="360" layout="horizontal" @change="proxyDraw" />
      </t-space>
    </div>
    <t-form>
      <t-form-item label="操作">
        <t-space>
          <SelectChara @select="handleSelect" />
          <t-space :size="5">
            <t-button shape="square" theme="default" @click="handleCopyImage">
              <CopyIcon />
            </t-button>
            <t-button shape="square" theme="default" @click="handleDownloadImage">
              <DownloadIcon />
            </t-button>
          </t-space>
        </t-space>
      </t-form-item>
      <t-form-item label="显示文字">
        <t-textarea v-model="currentConf.text" :maxlength="30" :autosize="true" @change="handleText" />
      </t-form-item>
      <t-form-item label="字体尺寸">
        <t-slider v-model="currentConf.fontSize" :input-number-props="true" :max="100" :min="5" @change="proxyDraw" />
      </t-form-item>
      <t-form-item v-if="textMultipleLines" label="行间距">
        <t-slider v-model="currentConf.spaceSize" :input-number-props="true" :max="120" :min="1" @change="proxyDraw" />
      </t-form-item>
      <t-form-item label="旋转">
        <t-slider v-model="currentConf.rotate" :input-number-props="true" :max="63" :min="0" @change="proxyDraw" />
      </t-form-item>
      <t-form-item label="曲度" help="非长字符串渲染时使用">
        <t-switch v-model="currentConf.curve" @change="proxyDraw" />
      </t-form-item>
      <t-form-item label="商业字体" help="商业字体仅供非商业用途使用">
        <t-switch v-model="currentConf.useCommercialFonts" @change="proxyDraw" />
      </t-form-item>
    </t-form>
  </content-layout>
</template>

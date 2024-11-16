<script lang="tsx" setup>
import { CopyIcon, DownloadIcon } from "tdesign-icons-vue-next";
import { b64ToBlob } from "@/lib/util/imageTransform";
import { usePjskStore } from "@/pages/draw/pjsk-sticker/scripts/store";
import CharacterList from "@/assets/conf/characters.json";
import StickerCanvas from "@/pages/draw/pjsk-sticker/comps/StickerCanvas.vue";
import SelectChara from "@/pages/draw/pjsk-sticker/comps/SelectChara.vue";
import type { CharacterDefinition } from "@/lib/type/typeSticker";
import { copyImage, downloadImage } from "@/lib/util/imageUtil";
import { onMounted, ref, computed } from "vue";
import { useKeyUpdate } from "@/lib/composable/useKeyUpdate";
import { storeToRefs } from "pinia";
import { get, set } from "@vueuse/core";

const charaList: CharacterDefinition[] = CharacterList;

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

const store = usePjskStore();
const { charaId, fontSize, spaceSize, rotate, x, y, text, curve, useCommercialFonts } = storeToRefs(store);

const yProxy = computed({
  get: () => 360 - get(y),
  set: value => {
    set(y, 360 - value);
  },
});

const stickerCanvasRef = ref();
const { key } = useKeyUpdate();

const textMultipleLines = computed(() => text.value.includes("\n"));

const updateCurrentConf = (id: number) => {
  const chara = charaList[id];
  store.$patch({
    charaId: id,
    fontSize: chara.defaultText.s,
    rotate: chara.defaultText.r,
    x: chara.defaultText.x,
    y: chara.defaultText.y,
    text: chara.defaultText.text,
  });
};

const proxyDraw = () => {
  //updateKey();
};
const handleText = () => {
  if (textMultipleLines.value) {
    set(spaceSize, 52);
  }
  proxyDraw();
};
const handleSelect = (index: number) => {
  set(charaId, index);
  updateCurrentConf(index);
  proxyDraw();
};
const handleCopyImage = async () => {
  const canvas = document.getElementById("sticker-canvas") as HTMLCanvasElement;
  await copyImage(b64ToBlob(canvas?.toDataURL().split(",")[1]));
};
const handleDownloadImage = async () => {
  const canvas = document.getElementById("sticker-canvas") as HTMLCanvasElement;
  await downloadImage(canvas.toDataURL(), `${charaList[get(charaId)].name}_sticker.png`);
};

onMounted(() => {
  updateCurrentConf(get(charaId));
});
</script>

<template>
  <content-layout title="PJSK表情" :subtitle="subtitle">
    <div class="mb-3 mt-1" style="text-align: center">
      <t-space :size="8" direction="vertical">
        <t-space :size="16" direction="horizontal">
          <div style="height: 300px; width: 300px">
            <StickerCanvas :key="key" ref="stickerCanvasRef" />
          </div>
          <t-slider v-model="yProxy" :max="360" layout="vertical" @change="proxyDraw" />
        </t-space>
        <t-slider v-model="x" :max="360" layout="horizontal" @change="proxyDraw" />
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
        <t-textarea v-model="text" :maxlength="30" :autosize="true" @change="handleText" />
      </t-form-item>
      <t-form-item label="字体尺寸">
        <t-slider v-model="fontSize" :input-number-props="true" :max="100" :min="5" @change="proxyDraw" />
      </t-form-item>
      <t-form-item v-if="textMultipleLines" label="行间距">
        <t-slider v-model="spaceSize" :input-number-props="true" :max="120" :min="1" @change="proxyDraw" />
      </t-form-item>
      <t-form-item label="旋转">
        <t-slider v-model="rotate" :input-number-props="true" :max="63" :min="0" @change="proxyDraw" />
      </t-form-item>
      <t-form-item label="曲度" help="非长字符串渲染时使用">
        <t-switch v-model="curve" @change="proxyDraw" />
      </t-form-item>
      <t-form-item label="商业字体" help="商业字体仅供非商业用途使用">
        <t-switch v-model="useCommercialFonts" @change="proxyDraw" />
      </t-form-item>
    </t-form>
  </content-layout>
</template>

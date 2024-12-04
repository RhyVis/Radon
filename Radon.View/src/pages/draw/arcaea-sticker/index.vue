<script lang="ts" setup>
import { useKeyUpdate } from "@/composable/useKeyUpdate.ts";
import { b64ToBlob } from "@/lib/util/imageTransform";
import { copyImage, downloadImage } from "@/lib/util/imageUtil";
import { arcaeaCharaList } from "@/pages/draw/arcaea-sticker/scripts/define.ts";
import { useArcaeaStickerStore } from "@/pages/draw/arcaea-sticker/scripts/store.ts";
import SelectChara from "@/pages/draw/pjsk-sticker/comps/SelectChara.vue";
import StickerCanvas from "@/pages/draw/pjsk-sticker/comps/StickerCanvas.vue";
import { get, set } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { CopyIcon, DownloadIcon } from "tdesign-icons-vue-next";
import { computed, onMounted } from "vue";

const store = useArcaeaStickerStore();
const { charaId, fontSize, spaceSize, rotate, x, y, text, curve, useCommercialFonts } = storeToRefs(store);
const { key, updateKey } = useKeyUpdate();

const yProxy = computed({
  get: () => 360 - get(y),
  set: value => {
    set(y, 360 - value);
  },
});
const textMultipleLines = computed(() => text.value.includes("\n"));

const canvasEl = () => document.getElementById("sticker-canvas") as HTMLCanvasElement;
const handleUpdate = (id: number) => {
  const chara = arcaeaCharaList[id];
  store.$patch({
    charaId: id,
    fontSize: chara.defaultText.s,
    rotate: chara.defaultText.r,
    x: chara.defaultText.x,
    y: chara.defaultText.y,
    text: chara.defaultText.text,
  });
};
const handleText = () => {
  if (textMultipleLines.value && get(spaceSize) < 52) {
    set(spaceSize, 52);
  }
};
const handleSelect = (index: number) => {
  set(charaId, index);
  handleUpdate(index);
};
const handleCopyImage = async () => await copyImage(b64ToBlob(canvasEl().toDataURL().split(",")[1]));
const handleDownloadImage = async () =>
  await downloadImage(canvasEl().toDataURL(), `${arcaeaCharaList[get(charaId)].name}_sticker.png`);

store.$subscribe(() => updateKey());

onMounted(() => handleUpdate(get(charaId)));
</script>

<template>
  <content-layout subtitle="资源来自Rosemoe/arcaea-stickers" title="Arcaea表情">
    <div class="mb-3 mt-1" style="text-align: center">
      <t-space :size="8" direction="vertical">
        <t-space :size="16" direction="horizontal">
          <div style="height: 300px; width: 300px">
            <StickerCanvas
              :key="key"
              :chara-id="charaId"
              :chara-list="arcaeaCharaList"
              :curve="curve"
              :font-size="fontSize"
              :rotate="rotate"
              :space-size="spaceSize"
              :text="text"
              :use-commercial-fonts="useCommercialFonts"
              :x="x"
              :y="y"
              res-endpoint="arcaea-sticker"
            />
          </div>
          <t-slider v-model="yProxy" :max="360" layout="vertical" />
        </t-space>
        <t-slider v-model="x" :max="360" layout="horizontal" />
      </t-space>
    </div>
    <t-form>
      <t-form-item label="操作">
        <t-space>
          <SelectChara :chara-list="arcaeaCharaList" res-endpoint="arcaea-sticker" @select="handleSelect" />
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
        <t-textarea v-model="text" :autosize="true" :maxlength="30" @change="handleText" />
      </t-form-item>
      <t-form-item label="字体尺寸">
        <t-slider v-model="fontSize" :input-number-props="true" :max="100" :min="5" />
      </t-form-item>
      <t-form-item v-if="textMultipleLines" label="行间距">
        <t-slider v-model="spaceSize" :input-number-props="true" :max="120" :min="1" />
      </t-form-item>
      <t-form-item label="旋转">
        <t-slider v-model="rotate" :input-number-props="true" :max="63" :min="0" />
      </t-form-item>
      <t-form-item help="非长字符串渲染时使用" label="曲度">
        <t-switch v-model="curve" />
      </t-form-item>
      <t-form-item help="商业字体仅供非商业用途使用" label="商业字体">
        <t-switch v-model="useCommercialFonts" />
      </t-form-item>
    </t-form>
  </content-layout>
</template>

<script setup lang="ts">
import { useNarrow } from "@/composable/useNarrow.ts";
import { get, useOnline } from "@vueuse/core";
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { useRouter } from "vue-router";

const { push } = useRouter();
const { t } = useI18n();
const online = useOnline();
const narrow = useNarrow();
const style = computed(() => ({
  width: get(narrow) ? "85%" : "400px",
  minHeight: "200px",
  height: "fit-content",
  maxHeight: "850px",
}));
</script>

<template>
  <content-layout title="404">
    <div class="text-center mt-2">
      <t-space direction="vertical" :size="24">
        <t-text class="r-no-select font-bold">{{ t("st") }}</t-text>
        <t-text class="font-bold" v-if="!online">{{ t("offline") }}</t-text>
        <t-image
          v-if="online"
          src="https://api.r10086.com/樱道随机图片api接口.php?图片系列=猫娘1"
          fit="contain"
          shape="round"
          :style="style"
        />
        <t-button theme="warning" shape="round" variant="base" @click="push('/')">
          {{ t("back") }}
        </t-button>
      </t-space>
    </div>
  </content-layout>
</template>

<i18n lang="yaml" locale="en">
st: "Oops, the page you're looking for doesn't exist"
offline: "It seems that you are offline"
back: "Back to Home"
</i18n>

<i18n lang="yaml" locale="zh-CN">
st: "哎呀，找不到这个页面呢"
offline: "看起来你处于离线状态"
back: "点此返回主页"
</i18n>

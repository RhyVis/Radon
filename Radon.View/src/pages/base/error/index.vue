<script setup lang="ts">
import { useI18n } from "vue-i18n";
import { useRouter } from "vue-router";

const { push } = useRouter();
const { t } = useI18n();
</script>

<template>
  <content-layout title="404">
    <div class="r-err-container mt-2">
      <t-space direction="vertical" :size="24">
        <t-text class="r-err-desc">{{ t("st") }}</t-text>
        <t-image
          src="https://api.r10086.com/樱道随机图片api接口.php?图片系列=猫娘1"
          fit="contain"
          shape="round"
          style="width: 400px; min-height: 200px; height: fit-content; max-height: 850px"
        />
        <t-button theme="warning" shape="round" variant="base" @click="push('/')">
          {{ t("back") }}
        </t-button>
      </t-space>
    </div>
  </content-layout>
</template>

<style scoped lang="less">
@import "@/assets/style/mixin.less";

.r-err-container {
  text-align: center;
  .r-err-desc {
    font-weight: bold;
    .r-no-select();
  }
}
</style>

<i18n lang="yaml" locale="en">
st: "Oops, the page you're looking for doesn't exist"
back: "Back to Home"
</i18n>

<i18n lang="yaml" locale="zh-CN">
st: "哎呀，找不到这个页面呢"
back: "点此返回主页"
</i18n>

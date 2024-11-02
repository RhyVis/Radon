<script lang="ts" setup>
import LoadStatus from "@/components/utility/LoadStatus.vue";
import VersionCheck from "@/components/utility/VersionCheck.vue";
import CardLayout from "@/layout/frame/ContentLayout.vue";
import { useGlobalStore } from "@/store/global";

const global = useGlobalStore();
</script>

<template>
  <CardLayout title="主页">
    <t-title level="h4">简介</t-title>
    <t-paragraph>
      <t-text>不会还有人需要我教怎么用吧，不会吧不会吧</t-text>
    </t-paragraph>
    <t-title level="h4">提示</t-title>
    <t-paragraph>
      <t-text>建议在本站禁用字体渲染的脚本与插件，会导致应用自定义字体渲染图像的功能异常</t-text>
    </t-paragraph>
    <t-title level="h4">加载状态</t-title>
    <t-paragraph>
      <LoadStatus :loading="!global.fontLoaded" label="字体" name="font" />
    </t-paragraph>
    <t-title level="h4">版本检查</t-title>
    <t-paragraph>
      <VersionCheck />
    </t-paragraph>
    <t-paragraph>
      <t-title level="h6">为什么会有版本检查？</t-title>
      <t-text>因为我玩不转服务端页面缓存设置，有时候管用有时候又不管用，摆了🤗</t-text>
    </t-paragraph>
  </CardLayout>
</template>

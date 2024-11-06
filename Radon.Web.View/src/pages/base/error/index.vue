<script setup lang="ts">
import CardLayout from "@/layout/frame/ContentLayout.vue";

const router = useRouter();
const { t } = useI18n();
</script>

<template>
  <CardLayout title="404">
    <div class="r-err-container mt-2">
      <t-space direction="vertical" :size="24">
        <t-text class="r-err-desc">{{ t("error.desc") }}</t-text>
        <t-image
          src="https://api.r10086.com/樱道随机图片api接口.php?图片系列=猫娘1"
          fit="contain"
          shape="round"
          style="width: 400px; min-height: 200px; height: fit-content; max-height: 850px"
        />
        <t-button theme="warning" shape="round" variant="base" @click="router.push('/')">
          {{ t("error.back") }}
        </t-button>
      </t-space>
    </div>
  </CardLayout>
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
error:
  desc: "Oops, the page you're looking for doesn't exist"
  back: "Back to Home"
</i18n>

<i18n lang="yaml" locale="zh-CN">
error:
  desc: "哎呀，找不到这个页面呢"
  back: "点此返回主页"
</i18n>

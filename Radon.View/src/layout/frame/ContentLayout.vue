<script setup lang="ts">
import { HomeIcon } from "tdesign-icons-vue-next";
import type { TdCardProps } from "tdesign-vue-next";
import { useRouter } from "vue-router";

const router = useRouter();

defineProps<{
  title?: string | TdCardProps["title"];
  subtitle?: string | TdCardProps["subtitle"];
  loading?: boolean;
}>();
</script>

<template>
  <div class="mt-sm-1 mt-md-1 mt-lg-1 mt-xl-2 mt-xxl-2 mb-2" style="margin: auto">
    <t-card class="r-ct-layout-card" :title="title" :subtitle="subtitle" :loading="loading" :header-bordered="true">
      <slot />
      <template #footer>
        <slot name="footer" />
      </template>
      <template #actions>
        <t-space :size="4">
          <slot name="actions">
            <t-button class="r-no-select" variant="text" theme="primary" shape="circle" @click="router.push('/')">
              <HomeIcon />
            </t-button>
          </slot>
        </t-space>
      </template>
    </t-card>
  </div>
</template>

<style scoped>
.r-ct-layout-card {
  align-content: center;
  width: 99%;
  margin: auto;
}
</style>

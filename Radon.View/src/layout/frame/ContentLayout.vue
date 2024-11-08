<script setup lang="ts">
import { HomeIcon } from "tdesign-icons-vue-next";
import type { TdCardProps } from "tdesign-vue-next";

defineProps<{
  title?: string | TdCardProps["title"];
  subtitle?: string | TdCardProps["subtitle"];
}>();
</script>

<template>
  <div class="mt-sm-1 mt-md-1 mt-lg-1 mt-xl-2 mt-xxl-2 mb-2" style="margin: auto">
    <t-card class="r-ct-layout-card" :title="title" :subtitle="subtitle" :header-bordered="true">
      <slot />
      <template #actions>
        <t-space :size="6">
          <slot name="actions">
            <RouterLink to="/">
              <t-button variant="text" theme="primary" shape="circle">
                <HomeIcon />
              </t-button>
            </RouterLink>
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

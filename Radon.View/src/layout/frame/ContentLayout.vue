<script lang="ts" setup>
import BtnHome from "@/components/btn/BtnHome.vue";
import type { TdCardProps } from "tdesign-vue-next";

defineProps<{
  title?: string | TdCardProps["title"];
  subtitle?: string | TdCardProps["subtitle"];
  loading?: boolean;
}>();
</script>

<template>
  <div class="mb-1 mt-0.5">
    <t-card
      class="m-auto w-full content-center"
      :header-bordered="true"
      :loading="loading"
      :subtitle="subtitle"
      :title="title"
    >
      <slot />
      <template #footer>
        <slot name="footer" />
      </template>
      <template #actions>
        <t-space :size="4">
          <slot name="actions">
            <BtnHome />
          </slot>
        </t-space>
      </template>
    </t-card>
  </div>
</template>

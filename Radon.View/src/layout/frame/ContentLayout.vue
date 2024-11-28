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
  <div class="mt-0.5 mb-1">
    <t-card
      class="w-full content-center m-auto"
      :title="title"
      :subtitle="subtitle"
      :loading="loading"
      :header-bordered="true"
    >
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

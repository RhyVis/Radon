<script setup lang="ts">
import { Content as TContent } from "tdesign-vue-next";
</script>

<template>
  <t-layout class="r-footer-override">
    <t-header id="base-header">
      <slot name="header" />
    </t-header>
    <t-content class="r-limit-height" id="base-content">
      <slot name="side" />
      <slot />
    </t-content>
    <t-footer id="base-footer">
      <slot name="footer" />
    </t-footer>
  </t-layout>
</template>

<style scoped>
.r-footer-override :deep(.t-layout__footer) {
  justify-content: center;
  padding: 0;
}

:global(.r-limit-height) {
  flex: 1;
  max-height: 100vh;
  overflow: hidden;
  overflow-y: auto;
  scrollbar-width: thin;
}
</style>

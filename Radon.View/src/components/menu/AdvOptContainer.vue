<script setup lang="ts">
import { useToggle } from "@vueuse/core";
import { SwapLeftIcon, SwapRightIcon } from "tdesign-icons-vue-next";

const [show, setShow] = useToggle(false);
</script>

<template>
  <t-space>
    <t-button shape="circle" @click="setShow()">
      <SwapLeftIcon v-if="show" />
      <SwapRightIcon v-else />
    </t-button>
    <Transition name="move">
      <div v-if="show">
        <t-space align="center" direction="vertical">
          <slot />
        </t-space>
      </div>
    </Transition>
  </t-space>
</template>

<style scoped lang="less">
.move-enter-active {
  transition: all 0.3s ease-out;
}
.move-leave-active {
  transition: all 0.3s cubic-bezier(1, 0.5, 0.8, 1);
}
.move-enter-from,
.move-leave-to {
  transform: translateX(-20px);
  opacity: 0;
}
</style>

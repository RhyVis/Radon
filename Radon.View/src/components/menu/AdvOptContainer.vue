<script setup lang="ts">
import { SwapLeftIcon, SwapRightIcon } from "tdesign-icons-vue-next";

const showAdv = ref(false);
const handleAdv = () => {
  showAdv.value = !showAdv.value;
};
</script>

<template>
  <t-space>
    <t-button shape="circle" @click="handleAdv">
      <SwapLeftIcon v-if="showAdv" />
      <SwapRightIcon v-else />
    </t-button>
    <Transition name="move">
      <div v-if="showAdv">
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

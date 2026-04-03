mergeInto(LibraryManager.library, {
  CloseModal: function () {
    if (window.closeUnityModal) {
      window.closeUnityModal();
    }
  }
});